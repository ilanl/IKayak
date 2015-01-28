//
//  Booking.h
//  KayApp
//
//  Created by Ilan Levy on 10/11/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "JSONModel.h"
@interface Booking : JSONModel

@property (nonatomic, strong) NSString *KayakName;
@property int Type;
@property (nonatomic, strong) NSString *Day;
@property (nonatomic, strong) NSString *Time;
@property int State;
@property (nonatomic, strong) NSString *TripKey;
@property (nonatomic, strong) NSString *OutingDate;

-(NSDate*) getAbsoluteDate:(int) minsOffset;

@end
