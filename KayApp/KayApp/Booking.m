//
//  Booking.m
//  KayApp
//
//  Created by Ilan Levy on 10/11/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "Booking.h"

@implementation Booking

@synthesize KayakName;
@synthesize Type;
@synthesize Day;
@synthesize Time;
@synthesize State;
@synthesize TripKey;
@synthesize OutingDate;

-(NSDate*) getAbsoluteDate:(int) minsOffset
{
    NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
    [f setNumberStyle:NSNumberFormatterDecimalStyle];
    NSNumber * myNumber = [f numberFromString:self.OutingDate];
    NSDate *date = [NSDate dateWithTimeIntervalSince1970: myNumber.intValue];
    
    NSCalendar *calendar = [[NSCalendar alloc] initWithCalendarIdentifier:NSGregorianCalendar];
    NSDateComponents *components = [calendar components:NSYearCalendarUnit|NSMonthCalendarUnit|NSDayCalendarUnit fromDate:date];
    [components setHour:0];
    [components setMinute:0];
    date = [calendar dateFromComponents:components];
    
    NSArray *list = [self.Time componentsSeparatedByString:@":"];
    int hrs= [f numberFromString:[list objectAtIndex:0]].intValue;
    int mins = [f numberFromString:[list objectAtIndex:1]].intValue;
    int secs = (hrs * 60 + mins) * 60;
    NSDate *absoluteTripDate = [date dateByAddingTimeInterval: secs];
    
    NSDate* absoluteReminderDate = [absoluteTripDate dateByAddingTimeInterval:-minsOffset*60];
    
    return absoluteReminderDate;
}

@end
