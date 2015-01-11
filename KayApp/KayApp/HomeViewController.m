//
//  DRCViewController.m
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "HomeViewController.h"
#import "Global.h"
#import "TitleView.h";

@implementation HomeViewController
{
    
    NSArray *forecasts;
    NSArray *weatherImages;
    int index;
    BOOL isMenuOpened;
    UIActivityIndicatorView* activityView;
    Forecast *selectedForecast;
    WeatherView *weatherView;
}

@synthesize menuView;
@synthesize weatherImage;
@synthesize lblDay;
@synthesize lblHour;
@synthesize imgHome;
@synthesize btnMenu;

- (void)viewDidLoad
{
    [btnMenu setBackgroundImage:[UIImage imageNamed:@"MenuIcon"] forState:UIControlStateNormal];
    
    imgHome.image = [UIImage imageNamed:@"Home"];
    [self.navigationItem setHidesBackButton:TRUE];
    [AppLog Log:@"Home:: didLoad"];
    
    [super viewDidLoad];
    
    CGRect rect = CGRectMake(0, 0, 0, 0);
    weatherView = [[WeatherView alloc]initWithFrame:rect];
    weatherView.parent = self;
    [self.view addSubview:weatherView];
    
}

- (void)viewWillAppear:(BOOL)animated
{
    [self loadData];
    [self.navigationController setNavigationBarHidden:NO];
    
    [[NSNotificationCenter defaultCenter]
     addObserver:self
     selector:@selector(applicationDidBecomeActiveNotification:)
     name:UIApplicationDidBecomeActiveNotification
     object:[UIApplication sharedApplication]];
}

- (void)viewWillDisappear:(BOOL)animated {
    [[NSNotificationCenter defaultCenter]
     removeObserver:self
     name:UIApplicationDidBecomeActiveNotification
     object:[UIApplication sharedApplication]];
}

- (void)loadData {
    
    NSDate *now = [NSDate date];
    
    NSLog(@"Home: refresh data");
    
    if ([[Weather getInstance] lastViewed] == nil || [now timeIntervalSinceDate:[[Weather getInstance] lastViewed]] / 60 > 1)
    {
        activityView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
        [self.view addSubview: activityView];
        activityView.center = CGPointMake(100,200);
        [activityView startAnimating];
        
        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
        
                [[Weather getInstance] reload];
            
                dispatch_sync(dispatch_get_main_queue(), ^{
                    
                    [activityView stopAnimating];
            });
        });
    }
    forecasts = [[DbAdapter getInstance] getForecasts];
    if ([forecasts count]>0)
    {
        index = 0;
        selectedForecast = [forecasts objectAtIndex:0];
        [self setForecastToDisplay:selectedForecast];
    }
}

- (void)applicationDidBecomeActiveNotification:(NSNotification *)notification {
    // Do something here
    
    [self loadData];
    
    
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
}
-(void) prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender{
    self.navigationItem.backBarButtonItem=[[UIBarButtonItem alloc] initWithTitle:@"" style:UIBarButtonItemStylePlain target:nil action:nil];
}

-(void)setForecastToDisplay:(Forecast *) forecast
{
    [weatherView redrawForecast:forecast];
    [self reloadInputViews];
}

- (IBAction)handSwipe:(id)sender {
    if ([forecasts count] == 0)
        return;
    
    index++;
    
    selectedForecast = [forecasts objectAtIndex:index%[forecasts count]];
    [self setForecastToDisplay:selectedForecast];
    
}

- (IBAction)swipeLeft:(id)sender {
    if ([forecasts count] == 0)
        return;
    
    index--;
    
    selectedForecast = [forecasts objectAtIndex:index%[forecasts count]];
    [self setForecastToDisplay:selectedForecast];
}

- (IBAction)menuOpen:(id)sender
{
    if (isMenuOpened == YES)
    {
        [menuView removeFromSuperview];
        menuView = nil;
        isMenuOpened = NO;
    }
    else
    {
        //open menu
        isMenuOpened = YES;
        CGRect rect = CGRectMake(30, 208, 260, 150);
        menuView = [[MenuView alloc]initWithFrame:rect];
        menuView.parent = self;
        [self.view addSubview:menuView];
    }
}
@end
