//
//  BoatsTabBarViewController.m
//  Kayapp
//
//  Created by Ilan Levy on 12/29/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "BoatsTabBarViewController.h"
#import "UIColor+Hex.h"
#import "TitleView.h"

@interface BoatsTabBarViewController ()

@end

@implementation BoatsTabBarViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
	// Do any additional setup after loading the view.
    
    TitleView *iv = [[TitleView alloc] initWithFrame:CGRectMake(0, 0, 0, 0)];
    [iv setTitle:@"Boats" withTime:@""];
    
    iv.contentMode = UIViewContentModeCenter;
    [iv sizeToFit];
    self.navigationItem.titleView = iv;
    
    [[UITabBar appearance] setTintColor:[UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0]];
    [[UITabBar appearance] setBarTintColor:[UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0]];
    
    UITabBarController *tabBarController = (UITabBarController *)self;
    UITabBar *tabBar = tabBarController.tabBar;
    
    CGRect rect = [[UIScreen mainScreen] bounds];
    int tabBarHeight = 48;
    [[UITabBar appearance] setFrame:CGRectMake(0, rect.size.height-tabBarHeight, rect.size.width, tabBarHeight)];
    
    UITabBarItem *tabBarItem1 = [tabBar.items objectAtIndex:0];
    UITabBarItem *tabBarItem2 = [tabBar.items objectAtIndex:1];
    
    tabBarItem1.selectedImage = [[UIImage imageNamed:@"kayak-selected.png"] imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal ];
    tabBarItem1.image = [[UIImage imageNamed:@"Boats-Page-Kayak-Icon.png"] imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal ];
    tabBarItem2.selectedImage = [[UIImage imageNamed:@"surfski-selected.png"]imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal ];
    tabBarItem2.image = [[UIImage imageNamed:@"Boats-Page-Surfski-Icon.png"]imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal ];

}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
