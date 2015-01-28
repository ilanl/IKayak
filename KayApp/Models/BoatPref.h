//
//  DRCBoatPref.h
//  kayak
//
//  Created by Ilan Levy on 9/6/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface BoatPref : NSObject

@property (nonatomic) NSString *key;
@property (nonatomic) NSString *name; //Kayak 13, 12 etc...
@property (nonatomic) int priority;
@property (nonatomic) int type;

@end
