class BookingCreateOrder {
  final List<BookingCreateBusinessAppoint>? appoint;
  final BookingCreateOrderBusiness? order;
  final int promoID;

  BookingCreateOrder({
    this.appoint,
    this.order,
    required this.promoID,
  });
}

class BookingCreateBusinessAppoint {
  final int servID;
  final int empID;
  final String appStatus;

  BookingCreateBusinessAppoint({
    required this.servID,
    required this.empID,
    required this.appStatus,
  });
}

class BookingCreateOrderBusiness {
  final int custID;
  final DateTime createAt;
  final DateTime orderDate;

  BookingCreateOrderBusiness({
    required this.custID,
    required this.createAt,
    required this.orderDate,
  });
}