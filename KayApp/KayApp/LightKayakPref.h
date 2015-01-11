//
//  LightKayakPref.h
//  KayApp
//
//  Created by Ilan Levy on 9/22/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "JSONModel.h"

@interface LightKayakPref : JSONModel

@property (nonatomic, strong)NSString* Key;
@property (nonatomic, strong)NSString* Name;
@property int Type;
@property int Weight;

-(id)initWithParams:(NSString *)key withName:(NSString *) name withType:(int) type withWeight:(int) weight;

@end
