//
//  LoginResponse.h
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "BaseResponse.h"
#import "Set.h"

@interface PreferenceResponse : BaseResponse

@property (nonatomic,strong)Set* Set;
@property BOOL IsFrozen;
@property int Reminder;

@end
