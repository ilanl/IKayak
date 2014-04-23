//
//  LightTimePref.h
//  KayApp
//
//  Created by Ilan Levy on 9/22/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "JSONModel.h"

@interface LightTimePref : JSONModel

@property (nonatomic, strong)NSString* DayOfWeek;
@property int Time;
@property int Type;


-(id)initWithParams:(NSString *)dayOfWeek withTime:(int) time withType:(int) type;


@end
