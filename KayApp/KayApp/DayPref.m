//
//  DRCDayPref.m
//  kayak
//
//  Created by Ilan Levy on 9/6/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "DayPref.h"

@implementation DayPref

@synthesize name;
@synthesize Early;
@synthesize Mid;
@synthesize Late;

- (id)initWithParams :(NSString *)dayOfWeek

{
    self = [super init];
    
    if (self) {
        
        self.name = dayOfWeek;
        self.Early = 0;
        self.Mid = 0;
        self.Late = 0;
    }
    
    return self;
}

@end
