//
//  LightTimePref.m
//  KayApp
//
//  Created by Ilan Levy on 9/22/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "LightTimePref.h"

@implementation LightTimePref

@synthesize DayOfWeek;
@synthesize Time;
@synthesize Type;

-(id)initWithParams:(NSString *)dayOfWeek withTime:(int) time withType:(int) type{

    self = [super init];
    
    if (self) {
        
        //NSLog(@"Initialized with %@ %i %i",  dayOfWeek, time, type);
        self.DayOfWeek = dayOfWeek;
        self.Time = time;
        self.Type = type;
    }
    
    return self;

}

@end
