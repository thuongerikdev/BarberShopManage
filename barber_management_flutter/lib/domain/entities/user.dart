class User {
  final String name;
  final String userId;
  final String token;
  final String? email;
  final String? phoneNumber;
  final String? dateOfBirth;
  final String? gender;

  User({
    required this.name,
    required this.userId,
    required this.token,
    this.email,
    this.phoneNumber,
    this.dateOfBirth,
    this.gender,
  });
}