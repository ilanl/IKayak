//
//  BookingResponse.h
//  KayApp
//
//  Created by Ilan Levy on 10/11/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "BaseResponse.h"
#import "Booking.h"

@protocol Booking;

@interface BookingResponse : BaseResponse

@property (nonatomic, strong) NSMutableArray<Booking> *Bookings;

@end
