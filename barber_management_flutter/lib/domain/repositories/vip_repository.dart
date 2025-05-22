import 'package:barbermanagemobile/domain/entities/vip/vip.dart';

abstract class VipRepository {
  Future<List<Vip>> getVips();
}