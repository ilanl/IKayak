//
//  DRCUser.h
//  kayak
//
//  Created by Ilan Levy on 9/19/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface User : NSObject

@property (nonatomic, strong) NSString *name;
@property (nonatomic, strong) NSString *password;
@property int state;
@property int reminder;


-(id)initWithParams:(NSString *)name password:(NSString *) password;

@end
