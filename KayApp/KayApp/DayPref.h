//
//  DRCDayPref.h
//  kayak
//
//  Created by Ilan Levy on 9/6/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface DayPref : NSObject

@property (nonatomic) NSString *name; //Sunday, etc

@property int Early;
@property int Mid;
@property int Late;

-(id)initWithParams:(NSString *)dayOfWeek;

@end