//
//  LightKayakPref.m
//  KayApp
//
//  Created by Ilan Levy on 9/22/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "LightKayakPref.h"

@implementation LightKayakPref

@synthesize Key;
@synthesize Name;
@synthesize Type;
@synthesize Weight;

-(id)initWithParams:(NSString *)key withName:(NSString *) name withType:(int) type withWeight:(int) weight{
    self = [super init];
    
    if (self) {
        
        //NSLog(@"Initialized with %@ %@ %i %i", key, name, type, weight);
        self.Key = key;
        self.Name = name;
        self.Type = type;
        self.Weight = weight;
    }
    
    return self;


}

@end
