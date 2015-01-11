//
//  DayView.h
//  Kayapp
//
//  Created by Ilan Levy on 11/30/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DayPref.h"
#import "Global.h"
#import "DbAdapter.h"

@interface DayView : UIView
@property (weak, nonatomic) IBOutlet UIButton *btnSurf;
@property (weak, nonatomic) IBOutlet UIButton *btnKayak;
@property (weak, nonatomic) IBOutlet UILabel *lblTitle;

- (IBAction)UpdateSurfSki:(id)sender;
- (IBAction)UpdateKayak:(id)sender;
- (void) setPref:(DayPref *)pref;
@property bool isSurfSkiEnabled, isKayakEnabled;

@property (weak, nonatomic) IBOutlet UIButton *btnBorder;
@property int time;

@end
