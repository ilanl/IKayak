//
//  BaseResponse.h
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "JSONModel.h"
#import "Error.h"

@interface BaseResponse : JSONModel

@property (nonatomic, strong)NSString<Optional>* SecurityToken;
@property (nonatomic, strong)NSString* Status;
@property (nonatomic, strong)Error<Optional>* Error;

@end
