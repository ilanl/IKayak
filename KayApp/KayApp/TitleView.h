//
//  TitleView.h
//  Kayapp
//
//  Created by Ilan Levy on 11/29/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface TitleView : UIView
@property (weak, nonatomic) IBOutlet UILabel *lblDay;
@property (weak, nonatomic) IBOutlet UILabel *lblHour;

-(void) setTitle:(NSString*) day withTime:(NSString*) hour;

@end
