//
//  DayView.m
//  Kayapp
//
//  Created by Ilan Levy on 11/30/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "DayView.h"
#import "UIColor+Hex.h"

@implementation DayView
@synthesize lblTitle, btnKayak, btnSurf, btnBorder;
@synthesize time, isKayakEnabled, isSurfSkiEnabled;


NSArray* surfs;
NSArray* kayaks;
DayPref* myPref;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"DayView" owner:self options:nil];
        self = [nib objectAtIndex:0];
    }
    
    kayaks = [NSArray arrayWithObjects: @"Day-page-chosed-Kayak-button.png", @"Day-page-unchosed-Kayak-button.png",nil];
    
    surfs = [NSArray arrayWithObjects: @"Day-page-chosed-surfski-button.png", @"Day-page-unchosed-surfski-button.png",nil];
    
    
    [self.lblTitle setFont:[UIFont fontWithName:@"Roboto-Regular" size:19.0]];
    self.lblTitle.textColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    [self.btnBorder setBackgroundColor:[UIColor colorWithHexString:@"1593DB" withAlpha:1.0]];
    
    
    return self;
}

-(void) setPref:(DayPref *)pref{
    myPref = pref;
    int prefValue = 0;
    switch (self.time) {
        case 0:
            prefValue = myPref.Early;
            break;
        case 1:
            prefValue = myPref.Mid;
            break;
        case 2:
            prefValue = myPref.Late;
            break;
        default:
            break;
    }
    
    switch (prefValue) {
        case 0:
            isSurfSkiEnabled = false;
            isKayakEnabled = false;
            break;
        case 1:
            isKayakEnabled = false;
            isSurfSkiEnabled = true;
            break;
        case 2:
            isSurfSkiEnabled = false;
            isKayakEnabled = true;
            break;
        case 3:
            isKayakEnabled = true;
            isSurfSkiEnabled = true;
            break;
        default:
            break;
    }
    

    [self redraw];
}

-(void) redraw{
    
    [self.btnSurf setBackgroundImage:[UIImage imageNamed: [surfs objectAtIndex:(isSurfSkiEnabled ? 0 : 1)]] forState:UIControlStateNormal];
    
    [self.btnKayak setBackgroundImage:[UIImage imageNamed: [kayaks objectAtIndex:(isKayakEnabled ? 0 : 1)]] forState:UIControlStateNormal];
    
    [AppLog Log:@"day: '%@' %i %i %i", myPref.name, myPref.Early, myPref.Mid, myPref.Late, nil];
}

-(void)Update {
    switch (self.time) {
        case 0:
            [[DbAdapter getInstance] updateTimeWithParams:myPref.name withTime:time withType:myPref.Early];
            break;
        case 1:
            [[DbAdapter getInstance] updateTimeWithParams:myPref.name withTime:time withType:myPref.Mid];
            break;
        case 2:
            [[DbAdapter getInstance] updateTimeWithParams:myPref.name withTime:time withType:myPref.Late];
            break;
        default:
            break;
    }
    
   
    
}

- (IBAction)UpdateSurfSki:(id)sender {
    
    [AppLog Log:@"update surf ski"];
    isSurfSkiEnabled = !isSurfSkiEnabled;
    switch (self.time) {
        case 0:
            myPref.Early = [self GetPrefValue];
            break;
        case 1:
            myPref.Mid = [self GetPrefValue];
            break;
        case 2:
            myPref.Late = [self GetPrefValue];
            break;
        default:
            break;
    }
    [self Update];
    [self redraw];
    
}

-(int) GetPrefValue{
    int result;
    
    if (!isSurfSkiEnabled && !isKayakEnabled)
        result = 0;
    
    else if (isSurfSkiEnabled && !isKayakEnabled)
        result = 1;
    
    else if (!isSurfSkiEnabled && isKayakEnabled)
        result = 2;
    
    else result = 3;
    
    return result;
}

- (IBAction)UpdateKayak:(id)sender {
    
    [AppLog Log:@"update kayak"];
    isKayakEnabled = !isKayakEnabled;
    switch (self.time) {
        case 0:
            myPref.Early = [self GetPrefValue];
            break;
        case 1:
            myPref.Mid = [self GetPrefValue];
            break;
        case 2:
            myPref.Late = [self GetPrefValue];
            break;
        default:
            break;
    }
    [self Update];
    [self redraw];
}
@end
