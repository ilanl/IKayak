//
//  PreferenceRequest.h
//  KayApp
//
//  Created by Ilan Levy on 9/22/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "BaseRequest.h"
#import "Set.h"

@interface PreferenceRequest : BaseRequest

@property (nonatomic, strong)NSString* UserName;
@property (nonatomic, strong)NSString* Password;
@property (nonatomic, strong)NSString* Action;
@property (nonatomic, strong) Set* Set;
@property (nonatomic, strong)NSString* IsFrozen;
@property int Reminder;

@end
