class User {
  final String name;
  final String userId;
  final String token;
  final String? email;
  final String? phoneNumber;
  final String? dateOfBirth;
  final String? gender;
  final int? roleID;
  final String? userName;
  final String? avatar;
  final bool? isEmp;
  final bool? isEmailVerified;
  final String? emailVerificationToken;

  User({
    required this.name,
    required this.userId,
    required this.token,
    this.email,
    this.phoneNumber,
    this.dateOfBirth,
    this.gender,
    this.roleID,
    this.userName,
    this.avatar,
    this.isEmp,
    this.isEmailVerified,
    this.emailVerificationToken,
  });
}