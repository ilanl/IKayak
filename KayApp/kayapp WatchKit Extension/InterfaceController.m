#import "InterfaceController.h"
#import "Forecast.h"

@interface InterfaceController()
@property (weak, nonatomic) IBOutlet WKInterfaceButton *btnBookings;
@property (weak, nonatomic) IBOutlet WKInterfaceLabel *lblBookings;

@end

@implementation InterfaceController



- (void)awakeWithContext:(id)context {
    [super awakeWithContext:context];

    // Configure interface objects here.
}

- (void)willActivate {
    // This method is called when watch view controller is about to be visible to user
    [super willActivate];
}

- (void)didDeactivate {
    // This method is called when watch view controller is no longer visible
    [super didDeactivate];
}
- (IBAction)getForecastsWithBookings {
    
    NSDictionary *applicationData = [[NSDictionary alloc] initWithObjects:@[@"forecasts"] forKeys:@[@"request"]];
    
    [WKInterfaceController openParentApplication:applicationData reply:^(NSDictionary *replyInfo, NSError *error) {

        NSArray* data = [replyInfo objectForKey:@"forecasts"];
        NSLog(@"data: %@", data);
        
        [self.lblBookings setText:[NSString stringWithFormat:@"%lu", (unsigned long)[data count]]];
    }];
}

@end



