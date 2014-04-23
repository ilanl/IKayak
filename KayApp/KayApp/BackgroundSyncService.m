#import "BackgroundSyncService.h"


@implementation BackgroundSyncService

/*
 * The singleton instance. To get an instance, use
 * the getInstance function.
 */
static BackgroundSyncService *instance = NULL;
static NSString* baseUrl = NULL;

/**
 * Singleton instance.
 */
+(BackgroundSyncService *)getInstance {
    @synchronized(self) {
        if (instance == NULL) {
            instance = [[self alloc] init];
            
            baseUrl = [[NSBundle mainBundle] objectForInfoDictionaryKey:@"base_url"];
        }
    }
    return instance;
}

-(void) run
{
    User *user = [[DbAdapter getInstance] currentUser];
    Set *set = [[DbAdapter getInstance] getLocalPreferencesFromDb];
    NSString *deviceToken = [[DbAdapter getInstance] getDeviceToken];

    if (user.state == 1)
    {
        for (Booking* b in [[DbAdapter getInstance] getBookings])
        {
            [[DbAdapter getInstance] updateBookingState:b.TripKey withState:1];
        }
    }
    NSMutableArray* bookings = [[DbAdapter getInstance] getBookings];
    NSArray *keys = [[DbAdapter getInstance] getBookingsToCancel];
    
    dispatch_group_t group = dispatch_group_create();
    
    dispatch_group_async(group,dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^ {
        // block2
        [AppLog Log:@"saving preferences - start"];
        
        if ([[DbAdapter getInstance] isDirty] == TRUE)
        {
            PreferenceRequest *requestObj = [PreferenceRequest new];
            
            [requestObj setAction:@"1"];
            [requestObj setUserName:user.name];
            [requestObj setPassword:user.password];
            [requestObj setDeviceToken: deviceToken];
            
            if ([[Global sharedSingleton] statusHasChanged])
                [requestObj setIsFrozen:user.state != 0 ? @"1" : @"0"];
            else
                [requestObj setIsFrozen:@""];
            
            int reminder = user.reminder;
            [requestObj setReminder:reminder];
            [requestObj setSet: set];
            
            NSString *jsonString = [requestObj toJSONString];
            NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@/preferences.ashx", baseUrl]];
            
            __weak __block ASIHTTPRequest *req = [ASIHTTPRequest requestWithURL:url];
            
            [req addRequestHeader:@"User-Agent" value:@"ASIHTTPRequest"];
            [req addRequestHeader:@"Content-Type" value:@"application/json"];
            [req appendPostData:[jsonString  dataUsingEncoding:NSUTF8StringEncoding]];
            [req setTimeOutSeconds:60];
            
            [req startSynchronous];
            NSError *error = [req error];
            if (error)
            {
                [AppLog Log:@"request failed"];
            }
        }
        [AppLog Log:@"saving preferences - end"];
    });
    
    dispatch_group_async(group,dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^ {
        // block1
        
        if ([keys count]==0)
            return;
        
        [AppLog Log:@"saving booking operations - start"];
        BookingRequest *requestObj = [BookingRequest new];

        [requestObj setAction:@"4"]; //cancel
        [requestObj setUserName:user.name];
        [requestObj setPassword:user.password];
        [requestObj setDeviceToken: deviceToken];
        
        [requestObj setKeys:keys];
        
        NSString *jsonString = [requestObj toJSONString];
        NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@/bookings.ashx", baseUrl]];
        
        ASIHTTPRequest *req = [ASIHTTPRequest requestWithURL:url];
        
        [req addRequestHeader:@"User-Agent" value:@"ASIHTTPRequest"];
        [req addRequestHeader:@"Content-Type" value:@"application/json"];
        [req appendPostData:[jsonString  dataUsingEncoding:NSUTF8StringEncoding]];
        
        [req setTimeOutSeconds:60];
        
        [req startSynchronous];
        NSError *error = [req error];
        if (error)
        {
            [AppLog Log:@"request failed"];
        }
        [[DbAdapter getInstance] cleanUpUnActiveBookings];
        [AppLog Log:@"saving booking operations - end"];
    });
    
    
    dispatch_group_async(group,dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_BACKGROUND, 0), ^ {
        // block1
        [AppLog Log:@"syncing reminders - start"];
        @try
        {
            BOOL isEnabled = NO;
            if (user.state == 0)
            {
                isEnabled = YES;
            }
            [[Reminder getInstance] syncEvents: bookings isEnabled:isEnabled withOffSet:user.reminder];
        }
        @catch (NSException *exception) {
            
        }
        @finally {
            
        }
        [AppLog Log:@"syncing reminders - end"];
    });
    
    dispatch_group_wait(group, DISPATCH_TIME_FOREVER);
    
    [[Global sharedSingleton] setStatusHasChanged:false];
}

-(void) reloadBookings
{
    [AppLog Log:@"reload bookings"];
    
    [[DbAdapter getInstance] getCurrentUser];
    NSString *encodedUser = [[[DbAdapter getInstance] currentUser] name];
    NSString *encodedPassword = [[[DbAdapter getInstance] currentUser] password];
    
    BookingRequest *requestObj = [BookingRequest new];
    
    [requestObj setAction:@"0"];
    [requestObj setUserName:encodedUser];
    [requestObj setPassword:encodedPassword];
    [requestObj setDeviceToken: [[DbAdapter getInstance] getDeviceToken]];
    
    NSString *jsonString = [requestObj toJSONString];
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@/bookings.ashx", baseUrl]];
    __weak ASIHTTPRequest *req = [ASIHTTPRequest requestWithURL: url];
    
    [req addRequestHeader:@"User-Agent" value:@"ASIHTTPRequest"];
    [req addRequestHeader:@"Content-Type" value:@"application/json"];
    [req appendPostData:[jsonString  dataUsingEncoding:NSUTF8StringEncoding]];
    
    [req setTimeOutSeconds:60];
    
    [req startSynchronous];
    NSString *responseString = [req responseString];
    NSError* err;
    BookingResponse *responseObj = [[BookingResponse alloc] initWithString:responseString error:&err];
    if (!err)
    {
        if ([[responseObj Status] isEqual: @"Success"])
        {
            [[DbAdapter getInstance] saveBookingsToLocal:responseObj];
        }
    }
}


@end