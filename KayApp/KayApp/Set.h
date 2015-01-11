//
//  Set.h
//  KayApp
//
//  Created by Ilan Levy on 9/22/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "JSONModel.h"
#import "LightKayakPref.h"
#import "LightTimePref.h"

@protocol LightKayakPref;
@protocol LightTimePref;

@interface Set : JSONModel

@property (nonatomic, strong) NSMutableArray<LightKayakPref> *KayakPrefs;
@property (nonatomic, strong) NSMutableArray<LightTimePref> *TimePrefs;

@end
