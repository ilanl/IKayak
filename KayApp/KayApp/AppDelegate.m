//
//  DRCAppDelegate.m
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "AppDelegate.h"
#import "DbAdapter.h"
#import <Pushbots/Pushbots.h>
#import "BookingViewController.h"
#import "UIColor+Hex.h"

#define UIColorFromRGB(rgbValue) [UIColor colorWithRed:((float)((rgbValue & 0xFF0000) >> 16))/255.0 green:((float)((rgbValue & 0xFF00) >> 8))/255.0 blue:((float)(rgbValue & 0xFF))/255.0 alpha:1.0]

@implementation AppDelegate
{

}
AppDelegate *appContext;


- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    if ([[[UIDevice currentDevice] systemVersion] floatValue] >= 7)
    {
        
        [[UINavigationBar appearance] setBarTintColor:[UIColor colorWithHexString:@"1593DB" withAlpha:1.0]];
        [[UINavigationBar appearance] setTintColor:[UIColor whiteColor]];
        [[UIBarButtonItem appearance] setBackButtonTitlePositionAdjustment:UIOffsetMake(-100, -100) forBarMetrics:UIBarMetricsDefault];
        
    }
    [Pushbots getInstance];
    
    NSString *appName = [[NSBundle bundleWithIdentifier:@"BundleIdentifier"] objectForInfoDictionaryKey:@"CFBundleExecutable"];
    appContext = self;
    [AppLog Log:@"didFinishLaunching"];
    
    // Let the device know we want to receive push notifications
	[[UIApplication sharedApplication] registerForRemoteNotificationTypes:
     (UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound | UIRemoteNotificationTypeAlert)];
    NSDictionary * userInfo = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
    if(userInfo) {
        // Notification Message
        NSString* notificationMsg = [userInfo valueForKey:@"message"];
        // Custom Field
        NSString* title = [userInfo valueForKey:@"title"];
        [AppLog Log:@"Notification Msg is %@ and Custom field title = %@", notificationMsg , title];
    }
    
    return YES;
}

-(void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)devToken
{
    NSString *token = [[devToken description] stringByTrimmingCharactersInSet:      [NSCharacterSet characterSetWithCharactersInString:@"<>"]];
    
    token = [token stringByReplacingOccurrencesOfString:@" " withString:@""];
    
    if (token != nil)
        [[DbAdapter getInstance] updateDeviceToken:token];
    
    [AppLog Log:@"didRegisterForRemoteNotificationsWithDeviceToken with token %@", token];
}

- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{
    [AppLog Log:@"Failed to get token %@", error];
}

-(void)onReceivePushNotification:(NSDictionary *) pushDict andPayload:(NSDictionary *)payload
{
    @try {
        [AppLog Log:@"onReceivePushNotification"];
        [self reloadBookings];
    }
    @catch (NSException *exception) {
        
    }
    @finally {
        
    }
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo
{
    @try {
        [AppLog Log:@"didReceiveRemoteNotification"];
        [self reloadBookings];
    }
    @catch (NSException *exception) {
        
    }
    @finally {
        
    }
}

-(void) reloadBookings
{
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
        
        [[BackgroundSyncService getInstance] reloadBookings];
        
        dispatch_sync(dispatch_get_main_queue(), ^{
            
            [[NSNotificationCenter defaultCenter] postNotificationName:@"reloadTheTable" object:nil];
            
            [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
            
            [AppLog Log:@"Notification read"];
            @try {
                // reset badge on the server
                [[Pushbots getInstance] OpenedNotification];
                [[Pushbots getInstance] resetBadgeCount];
            }
            @catch (NSException* exception)
            {
                [AppLog Log:@"Got exception: %@ Reason: %@", exception.name, exception.reason];
            }
            @finally {
                
            }
            
        });
    });
}

- (void)applicationWillResignActive:(UIApplication *)application
{
    // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
    // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
    
    [AppLog Log:@"applicationWillResignActive"];
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
    // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
    // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
    
    [AppLog Log:@"applicationDidEnterBackground"];
    
    // bgTask is instance variable
    NSAssert(self->bgTask == UIBackgroundTaskInvalid, nil);
    
    bgTask = [application beginBackgroundTaskWithExpirationHandler: ^{
        dispatch_async(dispatch_get_main_queue(), ^{
            [application endBackgroundTask:self->bgTask];
            self->bgTask = UIBackgroundTaskInvalid;
        });
    }];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        
        if ([application backgroundTimeRemaining] > 1.0) {
            // Start background service synchronously
            [[BackgroundSyncService getInstance] run];
        }
        
        [application endBackgroundTask:self->bgTask];
        self->bgTask = UIBackgroundTaskInvalid;
        
    });
    
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
    // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
    
    [AppLog Log:@"applicationWillEnterForeground"];
}

- (void)applicationDidBecomeActive:(UIApplication *)application
{
    // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
    
    [AppLog Log:@"applicationDidBecomeActive"];
    [AppLog Log:@"Badges: %i",
     [UIApplication sharedApplication].applicationIconBadgeNumber];
    if ([UIApplication sharedApplication].applicationIconBadgeNumber > 0)
    {
        [self reloadBookings];
    }

}

- (void)applicationWillTerminate:(UIApplication *)application
{
    // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
    [AppLog Log:@"applicationWillTerminate"];
}

- (void)application:(UIApplication *)app didReceiveLocalNotification:(UILocalNotification *)notification
{
    [AppLog Log:@"didReceiveLocalNotification"];
}

@end
