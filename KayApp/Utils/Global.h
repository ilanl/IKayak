//
//  Global.h
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "AppLog.h"

@interface Global : NSObject
{
}
+(Global *)sharedSingleton;

@property BOOL statusHasChanged;

@end
