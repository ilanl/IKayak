//
//  WeatherView.m
//  Kayapp
//
//  Created by Ilan Levy on 11/13/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "WeatherView.h"
#import "TitleView.h"

@implementation WeatherView
@synthesize parent;
@synthesize imgWeather, imgCompassArrow;
@synthesize lblTemp,lblWaterTemp,lblWaveLong,lblWindSpeed, lblSwellSecs, lblWaveHeight;

@synthesize lblUnitsLong, lblUnitsSwell, lblUnitsWind, lblUnitWaveHeight;

NSArray *arr;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"WeatherView" owner:self options:nil];
        self = [nib objectAtIndex:0];
        
        self.frame = CGRectMake(0,75, 320, 300);
        
        // Initialization code
        self.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:0.688]; //0.695
        
        self.userInteractionEnabled = YES;
        NSLog(@"weather control loaded");
        
    }
    
    return self;
}


#define DEGREES_TO_RADIANS(x) (M_PI * x / 180.0)

-(void) redrawForecast:(Forecast* ) f
{
    NSString *imgName = [ [[NSBundle mainBundle] objectForInfoDictionaryKey: f.Weather] stringByAppendingString:@".png"];
    
    imgWeather.image = [UIImage imageNamed: imgName];
    imgWeather.alpha = 1.0f;
    
    lblTemp.text = [[NSString alloc]initWithFormat:@"%@°",f.TempC];
    [lblTemp setFont:[UIFont fontWithName:@"Roboto-Light" size:26.5]];
    
    lblWaterTemp.text = [[NSString alloc]initWithFormat:@"%@",f.WaterTempC];
    [lblWaterTemp setFont:[UIFont fontWithName:@"Roboto-Regular" size:28]];
    
    [lblUnitWaveHeight setFont:[UIFont fontWithName:@"Roboto-Light" size:12]];
    lblWaveHeight.text = [[NSString alloc]initWithFormat:@"%@",f.WaveH];
    [lblWaveHeight setFont:[UIFont fontWithName:@"Roboto-Regular" size:22]];
    
    [lblUnitsWind setFont:[UIFont fontWithName:@"Roboto-Light" size:12]];
    lblWindSpeed.text = [[NSString alloc]initWithFormat:@"%@",f.WindF];
    [lblWindSpeed setFont:[UIFont fontWithName:@"Roboto-Regular" size:22]];
    
    [lblUnitsSwell setFont:[UIFont fontWithName:@"Roboto-Light" size:12]];
    lblSwellSecs.text = [[NSString alloc]initWithFormat:@"%@",f.SwellSecs];
    [lblSwellSecs setFont:[UIFont fontWithName:@"Roboto-Regular" size:22]];
    
    NSArray *items = @[@"N", @"NNE", @"NE", @"ENE",@"E",@"ESE",@"SE",@"SSE",@"S",@"SSW",@"SW",@"WSW",@"W",@"WNW",@"NW",@"NNW"];
    
    long windOrientationIndex = [items indexOfObject:f.WindDir];
    imgCompassArrow.transform =
    CGAffineTransformMakeRotation(DEGREES_TO_RADIANS((windOrientationIndex) * 360/16));
    
    TitleView *iv = [[TitleView alloc] initWithFrame:CGRectMake(0, 0, 0, 0)];
    [iv setTitle:f.Day withTime:f.Hour];
    iv.contentMode = UIViewContentModeCenter;
    //[iv sizeToFit];
    self.parent.navigationItem.titleView = iv;
    [self.parent.navigationItem.titleView layoutIfNeeded];
}

@end
