

class BookingCreateOrderRequestModel {
  final List<BookingCreateBusinessAppointModel>? appoint;
  final BookingCreateOrderBusinessModel? order;
  final int promoID;

  BookingCreateOrderRequestModel({
    this.appoint,
    this.order,
    required this.promoID,
  });

  // Chuyển từ object sang JSON
  Map<String, dynamic> toJson() => {
        'Appoint': appoint?.map((e) => e.toJson()).toList(),
        'Order': order?.toJson(),
        'PromoID': promoID,
      };

  // Parse từ JSON sang object (nếu API trả về)
  factory BookingCreateOrderRequestModel.fromJson(Map<String, dynamic> json) {
    return BookingCreateOrderRequestModel(
      appoint: json['Appoint'] != null
          ? (json['Appoint'] as List)
              .map((e) => BookingCreateBusinessAppointModel.fromJson(e))
              .toList()
          : null,
      order: json['Order'] != null
          ? BookingCreateOrderBusinessModel.fromJson(json['Order'])
          : null,
      promoID: json['PromoID'],
    );
  }
}

class BookingCreateBusinessAppointModel {
  // final int appID; // Đã comment trong C#, nên tôi cũng bỏ qua
  final int servID;
  final int empID;
  final String appStatus;

  BookingCreateBusinessAppointModel({
    // this.appID,
    required this.servID,
    required this.empID,
    required this.appStatus,
  });

  Map<String, dynamic> toJson() => {
        // 'appID': appID,
        'servID': servID,
        'empID': empID,
        'appStatus': appStatus,
      };

  factory BookingCreateBusinessAppointModel.fromJson(Map<String, dynamic> json) {
    return BookingCreateBusinessAppointModel(
      // appID: json['appID'],
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