import 'package:flutter/material.dart';

class LoginForm extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        TextField(decoration: InputDecoration(labelText: "Email")),
        TextField(decoration: InputDecoration(labelText: "Password")),
      ],
    );
  }
}