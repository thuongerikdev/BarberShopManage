class BookingOrder {
  final int orderID;
  final int custID;
  final double orderTotal;
  final String orderStatus;
  final DateTime createAt;
  final DateTime orderDate;
  final List<dynamic>? bookingAppointments; // Nullable, as API returns null
  final dynamic bookingInvoice; // Nullable, as API returns null
  final List<dynamic>? bookingOrderProducts; // Nullable, as API returns null
  final dynamic bookingReview; // Nullable, as API returns null

  BookingOrder({
    required this.orderID,
    required this.custID,
    required this.orderTotal,
    required this.orderStatus,
    required this.createAt,
    required this.orderDate,
    this.bookingAppointments,
    this.bookingInvoice,
    this.bookingOrderProducts,
    this.bookingReview,
  });

  factory BookingOrder.fromJson(Map<String, dynamic> json) {
    return BookingOrder(
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