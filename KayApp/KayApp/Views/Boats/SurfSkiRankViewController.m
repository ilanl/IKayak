//
//  SurfSkiRankViewController.m
//  KayApp
//
//  Created by Ilan Levy on 10/17/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "SurfSkiRankViewController.h"

@interface SurfSkiRankViewController ()

@end

@implementation SurfSkiRankViewController

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
    self.boats = [[DbAdapter getInstance] getKayaksByType:1];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
