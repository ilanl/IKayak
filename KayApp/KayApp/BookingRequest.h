//
//  BookingRequest.h
//  KayApp
//
//  Created by Ilan Levy on 10/11/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "BaseRequest.h"

@interface BookingRequest : BaseRequest

@property (nonatomic, strong)NSString* UserName;
@property (nonatomic, strong)NSString* Password;
@property (nonatomic, strong)NSString* Action;
@property (nonatomic, strong)NSArray* Keys;

@end
