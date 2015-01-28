//
//  DRCUser.m
//  kayak
//
//  Created by Ilan Levy on 9/19/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "User.h"

@implementation User

@synthesize password;
@synthesize name;
@synthesize state;
@synthesize reminder;


- (id)initWithParams :(NSString *)userName password:(NSString *) pwd

{
    self = [super init];
    
    if (self) {
        
        //NSLog(@"Initialized with %@ %@", name, pwd);
        self.name = name;
        self.password = pwd;
        
    }
    
    return self;
}


@end
