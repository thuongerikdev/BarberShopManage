// import 'package:barbermanagemobile/domain/entities/customer.dart';

class Customer {
  final int customerID;
  final int userID;
  final int vipID;
  final double? loyaltyPoints; // Nullable
  final String? customerType;
  final String? customerStatus;
  final double? totalSpent; // Nullable

  Customer({
    required this.customerID,
    required this.userID,
    required this.vipID,
    this.loyaltyPoints = 0.0, // Default to 0.0 if null
    this.customerType,
    this.customerStatus,
    this.totalSpent = 0.0, // Default to 0.0 if null
  });

  factory Customer.fromJson(Map<String, dynamic> json) {
    return Customer(
      customerID: json['customerID'] as int,
      userID: json['userID'] as int,
      vipID: json['vipID'] as int,
      loyaltyPoints: (json['loyaltyPoints'] as num?)?.toDouble() ?? 0.0, // Handle null
      customerType: json['customerType'] as String?,
      customerStatus: json['customerStatus'] as String?,
      totalSpent: (json['totalSpent'] as num?)?.toDouble() ?? 0.0, // Handle null
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
    };
  }
}