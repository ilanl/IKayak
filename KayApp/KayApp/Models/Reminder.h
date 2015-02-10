//
//  MyCalendar.h
//  KayApp
//
//  Created by Ilan Levy on 10/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <UIKit/UIKit.h>
#import <EventKit/EventKit.h>
#import "Booking.h"

@interface Reminder : NSObject

@property (nonatomic, strong) EKEventStore* eventStore;

+ (Reminder *)getInstance;

- (void)syncEvents:(NSMutableArray*) bookings isEnabled:(BOOL) enable withOffSet:(int) minutesOffset;

@end
