//
//  DRCDayDetailViewController.m
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "DayDetailViewController.h"
#import "TitleView.h"
#import "UIColor+Hex.h"
#import "DayView.h"

@interface DayDetailViewController ()

@end

@implementation DayDetailViewController
DayView *morning, *afternoon, *late;

@synthesize pref;

-(id) init
{
    self = [super init];
    if(self)
    {
        //do something
    }

    return self;

}

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
        
    }
    return self;
}

-(void) initializeViewWithDay: (DayPref *) myDay;
{
    self.pref = myDay;
    
    [AppLog Log:@"day: '%@' %i %i %i", pref.name, pref.Early, pref.Mid, pref.Late, nil];
}

#pragma mark Actions

- (void)viewDidLoad
{
    [super viewDidLoad];
    self.view.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    
    TitleView *iv = [[TitleView alloc] initWithFrame:CGRectMake(0, 0, 0, 0)];
    [iv setTitle:pref.name withTime:@""];
    iv.contentMode = UIViewContentModeCenter;
    [iv sizeToFit];
    self.navigationItem.titleView = iv;
    
    morning = [[DayView alloc]initWithFrame:CGRectMake(0,0,0, 0)];
    morning.frame = CGRectMake(0,120, 320, 60);
    morning.time = 0;
    morning.lblTitle.text = @"Morning";
    [morning setPref:pref];
    [self.view addSubview:morning];
    
    afternoon = [[DayView alloc]initWithFrame:CGRectMake(0,0,0, 0)];
    afternoon.frame = CGRectMake(0,220, 320, 60);
    afternoon.lblTitle.text = @"Afternoon";
    afternoon.time = 1;
    [afternoon setPref:pref];
    [self.view addSubview:afternoon];
    
    late = [[DayView alloc]initWithFrame:CGRectMake(0,0,0, 0)];
    late.frame = CGRectMake(0,320, 320, 60);
    late.lblTitle.text = @"Late";
    late.time = 2;
    [late setPref:pref];
    [self.view addSubview:late];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
