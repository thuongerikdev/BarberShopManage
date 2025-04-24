class BookingCreateOrderRequestModel {
  final List<BookingCreateBusinessAppointModel>? appoint;
  final BookingCreateOrderBusinessModel? order;
  final int promoID;

  BookingCreateOrderRequestModel({
    this.appoint,
    this.order,
    required this.promoID,
  });

  Map<String, dynamic> toJson() => {
        'appoint': appoint?.map((e) => e.toJson()).toList(),
        'order': order?.toJson(),
        'promoID': promoID,
      };

  factory BookingCreateOrderRequestModel.fromJson(Map<String, dynamic> json) {
    return BookingCreateOrderRequestModel(
      appoint: json['appoint'] != null
          ? (json['appoint'] as List)
              .map((e) => BookingCreateBusinessAppointModel.fromJson(e))
              .toList()
          : null,
      order: json['order'] != null
          ? BookingCreateOrderBusinessModel.fromJson(json['order'])
          : null,
      promoID: json['promoID'],
    );
  }
}

class BookingCreateBusinessAppointModel {
  final int servID;
  final int empID;
  final String appStatus;

  BookingCreateBusinessAppointModel({
    required this.servID,
    required this.empID,
    required this.appStatus,
  });

  Map<String, dynamic> toJson() => {
        'servID': servID,
        'empID': empID,
        'appStatus': appStatus,
      };

  factory BookingCreateBusinessAppointModel.fromJson(Map<String, dynamic> json) {
    return BookingCreateBusinessAppointModel(
      servID: json['servID'],
      empID: json['empID'],
      appStatus: json['appStatus'],
    );
  }
}

class BookingCreateOrderBusinessModel {
  final int custID;
  final DateTime createAt;
  final DateTime orderDate;

  BookingCreateOrderBusinessModel({
    required this.custID,
    required this.createAt,
    required this.orderDate,
  });

  Map<String, dynamic> toJson() => {
        'custID': custID,
        'createAt': createAt.toIso8601String(),
        'orderDate': orderDate.toIso8601String(),
      };

  factory BookingCreateOrderBusinessModel.fromJson(Map<String, dynamic> json) {
    return BookingCreateOrderBusinessModel(
      custID: json['custID'],
      createAt: DateTime.parse(json['createAt']),
      orderDate: DateTime.parse(json['orderDate']),
    );
  }
}