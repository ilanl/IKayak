//
//  BaseRequest.h
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "JSONModel.h"

@interface BaseRequest : JSONModel
@property (nonatomic, strong) NSString* SecurityToken;
@property (nonatomic, strong) NSString* DeviceToken;

@end
