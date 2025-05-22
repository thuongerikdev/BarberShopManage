import 'package:barbermanagemobile/domain/entities/auth/customer.dart';

class CustomerModel extends Customer {
  CustomerModel({
    required super.customerID,
    required super.userID,
    required super.vipID,
    super.loyaltyPoints = 0.0, // Nullable with default
    super.customerType,
    super.customerStatus,
    super.totalSpent = 0.0, // Nullable with default
  });

  factory CustomerModel.fromJson(Map<String, dynamic> json) {
    return CustomerModel(
      customerID: json['customerID'] as int,
      userID: json['userID'] as int,
      vipID: json['vipID'] as int,
      loyaltyPoints: (json['loyaltyPoints'] as num?)?.toDouble() ?? 0.0, // Handle null
      customerType: json['customerType'] as String?,
      customerStatus: json['customerStatus'] as String?,
      totalSpent: (json['totalSpent'] as num?)?.toDouble() ?? 0.0, // Handle null
    );
  }

  @override
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