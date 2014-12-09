//
//  DRCAccountViewController.m
//  KayApp
//
//  Created by Ilan Levy on 9/20/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "LoginViewController.h"


@interface LoginViewController ()

@end

@implementation LoginViewController
UIActivityIndicatorView *activityView;
@synthesize txtUser;
@synthesize txtPassword;
@synthesize btnLogin;

NSString* userEncoded;
NSString* pwdEncoded;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Check if login is still valid
        
        //If valid
        
    }
    return self;
}

- (void)viewDidLoad
{
    
    [super viewDidLoad];
	// Do any additional setup after loading the view.
}

- (void)viewDidAppear:(BOOL)animated
{
    // Do any additional setup after loading the view, typically from a nib.
    [self.navigationController setNavigationBarHidden:YES];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)LoginClick:(id)sender
{
    
    activityView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
    [self.view addSubview: activityView];
    activityView.center = CGPointMake(200,150);
    [activityView startAnimating];

    NSLog(@"Login clicked");
    
    userEncoded = (NSString *)CFBridgingRelease(CFURLCreateStringByAddingPercentEscapes(
                                                                                  NULL,
                                                                                  (CFStringRef)txtUser.text,
                                                                                  NULL,
                                                                                  (CFStringRef)@"!*'();:@&=+$,/?%#[]",
                                                                                  kCFStringEncodingUTF8 ));
    
    pwdEncoded = (NSString *)CFBridgingRelease(CFURLCreateStringByAddingPercentEscapes(
                                                                                                        NULL,
                                                                                                        (CFStringRef)txtPassword.text,
                                                                                                        NULL,
                                                                                                        (CFStringRef)@"!*'();:@&=+$,/?%#[]",
                                                                                                        kCFStringEncodingUTF8 ));
    
   [self downloadAll:userEncoded withPassword:pwdEncoded withActivityIndicator:activityView withController:self];
    
    
}

@end
