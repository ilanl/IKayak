//
//  DRCViewController.h
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Global.h"
#import "DbAdapter.h"
#import "Weather.h"
#import "MenuView.h"
#import "WeatherView.h"

@interface HomeViewController : UIViewController{
    UIBackgroundTaskIdentifier bgTask;
    MenuView *menuView;
    
}

@property (strong, nonatomic) IBOutlet UIImageView *imgHome;

-(void)setForecastToDisplay:(Forecast *) forecast;
- (IBAction)handSwipe:(id)sender;
- (IBAction)swipeLeft:(id)sender;
@property (weak, nonatomic) IBOutlet UIButton *btnMenu;

@property (nonatomic, retain) MenuView* menuView;

@property (weak, nonatomic) IBOutlet UIImageView *weatherImage;
@property (weak, nonatomic) IBOutlet UILabel *lblDay;
@property (weak, nonatomic) IBOutlet UILabel *lblHour;
- (IBAction)menuOpen:(id)sender;

@end
