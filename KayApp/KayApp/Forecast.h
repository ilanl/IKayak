//
//  Forecast.h
//  KayApp
//
//  Created by Ilan Levy on 10/14/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "JSONModel.h"

@interface Forecast : JSONModel
@property (nonatomic, strong) NSString *Date;
@property (nonatomic, strong) NSString *Weather;
@property (nonatomic, strong) NSString *Day;
@property (nonatomic, strong) NSString *Hour;
@property (nonatomic, strong) NSString *TempC;
@property (nonatomic, strong) NSString *WaterTempC;
@property (nonatomic, strong) NSString *WaveH;
@property (nonatomic, strong) NSString *WindDir;
@property (nonatomic, strong) NSString *SwellSecs;
@property (nonatomic, strong) NSString *WindF;

@end
