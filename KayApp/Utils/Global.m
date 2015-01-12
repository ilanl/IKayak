//
//  Global.m
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "Global.h"
#import "DbAdapter.h"
#import "Weather.h"

@implementation Global

@synthesize statusHasChanged;
static Global *sharedGlobal;
+ (Global *)sharedSingleton
{
    static Global *sharedGlobal;
        
    @synchronized(self)
    {
        if (!sharedGlobal){
            sharedGlobal = [[Global alloc] init];
            
            [DbAdapter getInstance];
            [Weather getInstance];
        }
        return sharedGlobal;
    }
    
}


@end
