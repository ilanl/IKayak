//
//  Set.m
//  KayApp
//
//  Created by Ilan Levy on 9/22/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "Set.h"

@implementation Set

@synthesize KayakPrefs;
@synthesize TimePrefs;

-(id)init{

    self = [super init];
    
    if (self) {
        self.TimePrefs = [[NSMutableArray alloc]init];
        self.KayakPrefs = [[NSMutableArray alloc]init];
    }
    
    return self;

}

@end
