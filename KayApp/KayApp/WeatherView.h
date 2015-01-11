//
//  WeatherView.h
//  Kayapp
//
//  Created by Ilan Levy on 11/13/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "UIColor+Hex.h"
#import "Forecast.h"

@interface WeatherView : UIView

@property (nonatomic, retain) UIViewController *parent;
@property (weak, nonatomic) IBOutlet UIImageView *imgWeather;
@property (weak, nonatomic) IBOutlet UILabel *lblTemp;
@property (weak, nonatomic) IBOutlet UILabel *lblWaterTemp;
@property (weak, nonatomic) IBOutlet UIImageView *imgWaterTemp;

@property (weak, nonatomic) IBOutlet UILabel *lblSwellSecs;
@property (weak, nonatomic) IBOutlet UILabel *lblUnitsLong;
@property (weak, nonatomic) IBOutlet UILabel *lblUnitWaveHeight;
@property (weak, nonatomic) IBOutlet UILabel *lblWaveHeight;

@property (weak, nonatomic) IBOutlet UILabel *lblUnitsSwell;

@property (weak, nonatomic) IBOutlet UILabel *lblUnitsWind;

-(void) redrawForecast:(Forecast* ) f;
@property (weak, nonatomic) IBOutlet UILabel *lblWindSpeed;
@property (weak, nonatomic) IBOutlet UILabel *lblWaveLong;
@property (weak, nonatomic) IBOutlet UIImageView *imgCompassArrow;


@end
