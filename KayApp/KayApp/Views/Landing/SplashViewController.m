//
//  SplashViewController.m
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "SplashViewController.h"
#import "AppDelegate.h"
#import "ASIHTTPRequest.h"
#import "PreferenceRequest.h"
#import "PreferenceResponse.h"
#import "ApiViewController.h"

@interface SplashViewController ()

@end

@implementation SplashViewController
UIActivityIndicatorView *activityView;

NSString* userEncoded;
NSString* pwdEncoded;

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
	
    [self checkCredentials];
}

- (void)viewWillAppear:(BOOL)animated
{
    NSLog(@"splash screen up again");
    
    //if forecast is dirty then retrieve again
}

- (void)checkCredentials
{
    User *user = [[DbAdapter getInstance] currentUser];
    if (user == nil || user.name == nil || user.password == nil)
    {
        [self performSegueWithIdentifier:@"goToLogin" sender:self];
        return;
    }
    
    activityView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
    [self.view addSubview: activityView];
    activityView.center = CGPointMake(100,200);
    [activityView startAnimating];

    NSLog(@"check credentials");
    
    [self downloadAll:user.name withPassword:user.password withActivityIndicator:activityView withController:self];
    
}

- (void)viewDidUnload{
    
    activityView = nil;
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
