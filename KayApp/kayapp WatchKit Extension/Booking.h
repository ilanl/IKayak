#import <Foundation/Foundation.h>
#import "JSONModel.h"

@interface Booking : JSONModel

@property int Type;
@property int State;
@property (nonatomic, strong) NSString *KayakName;
@property (nonatomic, strong) NSString *Day;
@property (nonatomic, strong) NSString *Time;
@property (nonatomic, strong) NSString *TripKey;
@property (nonatomic, strong) NSString *OutingDate;

@end
