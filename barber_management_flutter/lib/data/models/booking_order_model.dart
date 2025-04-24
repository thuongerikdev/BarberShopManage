import 'package:barbermanagemobile/domain/entities/booking_order.dart';

class BookingOrderModel extends BookingOrder {
  BookingOrderModel({
    required int orderID,
    required int custID,
    required double orderTotal,
    required String orderStatus,
    required DateTime createAt,
    required DateTime orderDate,
    List<dynamic>? bookingAppointments,
    dynamic bookingInvoice,
    List<dynamic>? bookingOrderProducts,
    dynamic bookingReview,
  }) : super(
          orderID: orderID,
          custID: custID,
          orderTotal: orderTotal,
          orderStatus: orderStatus,
          createAt: createAt,
          orderDate: orderDate,
          bookingAppointments: bookingAppointments,
          bookingInvoice: bookingInvoice,
          bookingOrderProducts: bookingOrderProducts,
          bookingReview: bookingReview,
        );

  factory BookingOrderModel.fromJson(Map<String, dynamic> json) {
    return BookingOrderModel(
      orderID: json['orderID'] as int,
      custID: json['custID'] as int,
      orderTotal: (json['orderTotal'] as num).toDouble(),
      orderStatus: json['orderStatus'] as String,
      createAt: DateTime.parse(json['createAt'] as String),
      orderDate: DateTime.parse(json['orderDate'] as String),
      bookingAppointments: json['bookingAppointments'] as List<dynamic>?,
      bookingInvoice: json['bookingInvoice'],
      bookingOrderProducts: json['bookingOrderProducts'] as List<dynamic>?,
      bookingReview: json['bookingReview'],
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'orderID': orderID,
      'custID': custID,
      'orderTotal': orderTotal,
      'orderStatus': orderStatus,
      'createAt': createAt.toIso8601String(),
      'orderDate': orderDate.toIso8601String(),
      'bookingAppointments': bookingAppointments,
      'bookingInvoice': bookingInvoice,
      'bookingOrderProducts': bookingOrderProducts,
      'bookingReview': bookingReview,
    };
  }
}