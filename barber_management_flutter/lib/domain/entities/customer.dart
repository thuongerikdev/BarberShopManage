class Customer {
  final int customerID;
  final int userID;
  final int vipID;
  final double loyaltyPoints;
  final String? customerType;
  final String? customerStatus;
  final double totalSpent;
  final double percentDiscount;

  Customer({
    required this.customerID,
    required this.userID,
    required this.vipID,
    required this.loyaltyPoints,
    this.customerType,
    this.customerStatus,
    required this.totalSpent,
    required this.percentDiscount,
  });

  factory Customer.fromJson(Map<String, dynamic> json) {
    return Customer(
      customerID: json['customerID'] as int,
      userID: json['userID'] as int,
      vipID: json['vipID'] as int,
      loyaltyPoints: (json['loyaltyPoints'] as num).toDouble(),
      customerType: json['customerType'] as String?,
      customerStatus: json['customerStatus'] as String?,
      totalSpent: (json['totalSpent'] as num).toDouble(),
      percentDiscount: (json['percentDiscount'] as num).toDouble(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'customerID': customerID,
      'userID': userID,
      'vipID': vipID,
      'loyaltyPoints': loyaltyPoints,
      'customerType': customerType,
      'customerStatus': customerStatus,
      'totalSpent': totalSpent,
      'percentDiscount': percentDiscount,
    };
  }
}