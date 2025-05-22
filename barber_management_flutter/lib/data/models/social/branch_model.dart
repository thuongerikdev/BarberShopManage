import 'package:barbermanagemobile/domain/entities/social/branch.dart';

class BranchModel extends Branch {
  BranchModel({
    required int branchID,
    required String branchName,
    required String branchType,
    required String branchStatus,
    required int branchArea,
    required String branchHotline,
    required String startWork,
    required String endWork,
    required String location,
    required String branchImage,
  }) : super(
          branchID: branchID,
          branchName: branchName,
          branchType: branchType,
          branchStatus: branchStatus,
          branchArea: branchArea,
          branchHotline: branchHotline,
          startWork: startWork,
          endWork: endWork,
          location: location,
          branchImage: branchImage,
        );

  factory BranchModel.fromJson(Map<String, dynamic> json) {
    return BranchModel(
      branchID: json['branchID'] as int,
      branchName: json['branchName'] as String,
      branchType: json['branchType'] as String,
      branchStatus: json['branchStatus'] as String,
      branchArea: json['branchArea'] as int,
      branchHotline: json['branchHotline'] as String,
      startWork: json['startWork'] as String,
      endWork: json['endWork'] as String,
      location: json['location'] as String,
      branchImage: json['branchImage'] as String,
    );
  }

  @override
  Map<String, dynamic> toJson() {
    return {
      'branchID': branchID,
      'branchName': branchName,
      'branchType': branchType,
      'branchStatus': branchStatus,
      'branchArea': branchArea,
      'branchHotline': branchHotline,
      'startWork': startWork,
      'endWork': endWork,
      'location': location,
      'branchImage': branchImage,
    };
  }
}