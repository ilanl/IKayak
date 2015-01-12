//
//  MyCalendar.m
//  KayApp
//
//  Created by Ilan Levy on 10/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "Reminder.h"
#import "AppLog.h"

@implementation Reminder
@synthesize eventStore;

static Reminder *instance = NULL;

+(Reminder *)getInstance {
    @synchronized(self) {
        if (instance == NULL) {
            instance = [[self alloc] init];
            
        }
    }
    return instance;
}


-(id) init
{
    self = [super init];
    
    if (self)
    {
        NSLog(@"Initialized reminder");
    }
    
    return self;
}

- (void) removeAllAlarms{
    
    NSArray* notifications = [[UIApplication sharedApplication] scheduledLocalNotifications];
    for (UILocalNotification* notification in notifications)
    {
        NSRange range = [notification.alertBody rangeOfString:@"kayapp"];
        if (range.length > 0)
        {
            [[UIApplication sharedApplication] cancelLocalNotification:notification];
        }
    }
}

- (void)syncEvents:(NSMutableArray*) bookings isEnabled:(BOOL) enable withOffSet:(int) minutesOffset
{
    [self removeAllAlarms];
    
    if (enable == YES && minutesOffset > 0)
    {
        for (Booking* b in bookings)
        {
            if (b.State != 0)
                continue;
            
            NSDate* absoluteReminderDate = [b getAbsoluteDate:minutesOffset];
            
            if ([[NSDate date] compare:absoluteReminderDate] == NSOrderedDescending)
            {
                NSDate* absoluteBookingDate = [b getAbsoluteDate:0];
                
                if ([[NSDate date] compare:absoluteBookingDate] == NSOrderedDescending)
                    continue;
                
                [self createReminder:[NSDate date] withBooking:b];
            }
            else
            {
            //NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
            //[formatter setDateFormat:@"dd MMM yyyy HH:mm"];
            
            //NSString* formattedDate = [formatter stringFromDate:absoluteReminderDate];
            //NSLog(@"absolute reminder date: %@", formattedDate);
            [self createReminder:absoluteReminderDate withBooking:b];
            }
        }
    }
}

-(void)createReminder:(NSDate*) absoluteReminderDate withBooking:(Booking*) booking
{
    NSDateFormatter* formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"dd MMM yyyy HH:mm"];
    NSString* body = [[NSString alloc]initWithFormat:@"kayapp %@\n at %@",booking.KayakName, [formatter stringFromDate:absoluteReminderDate]];
    
    UILocalNotification* notification = [[UILocalNotification alloc]init];
    notification.fireDate = absoluteReminderDate;
    notification.soundName = @"budda.mp3";
    notification.applicationIconBadgeNumber = 0;
    NSDictionary *userDict = [NSDictionary dictionaryWithObject:@"someValue"
                                                         forKey:@"someKey"];
    notification.userInfo = userDict;
    notification.alertAction = @"View";
    notification.timeZone = [NSTimeZone defaultTimeZone];
    notification.alertBody = body;
    [notification setRepeatInterval:0];
    
    [[UIApplication sharedApplication] scheduleLocalNotification:notification];
}




@end
