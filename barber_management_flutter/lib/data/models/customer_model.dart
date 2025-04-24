import 'package:barbermanagemobile/domain/entities/customer.dart';

class CustomerModel extends Customer {
  CustomerModel({
    required super.customerID,
    required super.userID,
    required super.vipID,
    required super.loyaltyPoints,
    super.customerType,
    super.customerStatus,
    required super.totalSpent,
    required super.percentDiscount,
  });

  factory CustomerModel.fromJson(Map<String, dynamic> json) {
    return CustomerModel(
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
      'percentDiscount': percentDiscount,
    };
  }
}