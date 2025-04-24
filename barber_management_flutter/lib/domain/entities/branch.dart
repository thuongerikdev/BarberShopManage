class Branch {
  final int branchID;
  final String branchName;
  final String branchType;
  final String branchStatus;
  final int branchArea;
  final String branchHotline;
  final String startWork;
  final String endWork;
  final String location;
  final String branchImage;

  Branch({
    required this.branchID,
    required this.branchName,
    required this.branchType,
    required this.branchStatus,
    required this.branchArea,
    required this.branchHotline,
    required this.startWork,
    required this.endWork,
    required this.location,
    required this.branchImage,
  });

  factory Branch.fromJson(Map<String, dynamic> json) {
    return Branch(
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